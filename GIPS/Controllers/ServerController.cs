using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LiteDB;

namespace GIPS.Controllers
{


    [RoutePrefix("api/v1")]
    public class FirstContactController : ApiController
    {
        readonly string FILE_NAME = "db.db";
        readonly string FIRST_CONTACT = "FirstContact";


        // GET api/v1/FirstContact
        [Route("FirstContact")]
        [HttpGet]
        public string FirstContact()
        {

            /*初回起動時に使われる
             * ユーザーIDを返すと同時に
             * 初回起動日をusageに保存する。
             */

            //userIDの生成
            Guid userId = Guid.NewGuid();
            //現在の時間を取得
            DateTime date = DateTime.Now;

            //DB処理
            AddUser(userId);

            //UsageLogにFirstContactを追加
            AddUsageLog(userId, date, FIRST_CONTACT);

            //stringでuseridを返す
            return userId.ToString();


        }



        // GET api/v1/Usage
        [Route("Usage")]
        [HttpPost]
        public void Usage(string userid, string action, DateTime date)
        {
            //string Userid , string Action ,DateTime Date 引数
            /*
             * 送られてきた
             *ユーザーIDとactionをDBに保存する。 
             */

            AddUsageLog(Guid.Parse(userid), date, action);
        }

        [Route("Usages")]
        [HttpPost]
        public void Usages(string userid, Usages usages)
        {

            /*
             *Usageで一回一回送れなかった場合に
             *まとめてDBに保存するために使う。
             */
            Guid userId = Guid.Parse(userid);
            string act = usages.Name;
            foreach (DateTime date in usages.Date)
            {
                AddUsageLog(userId,date,act);
            }
        }

        //ここから関数

        //userIDをTableに追加する関数
        void AddUser(Guid userId)
        {
            using (var db = new LiteDatabase(FILE_NAME))
            {
                var usertable = db.GetCollection<UserClass>("Users");
                UserClass user = new UserClass { ID = userId };
                usertable.Insert(user);
            }
        }

        //UsageLogTableにログを追加するための関数
        void AddUsageLog(Guid userId, DateTime date,string act)
        {
            using (var db = new LiteDatabase(FILE_NAME))
            {
            

                var usageLogTable = db.GetCollection<UsageLog>("UsageLogs");
                var usageTable = db.GetCollection<UsageClass>("Usages");

                //UsageにActionが存在するかを確認し、なければ追加する
                if (usageTable.FindOne(x => x.Name.Equals(act)) == null)
                {
                    Guid id = Guid.NewGuid();
                    UsageClass usage = new UsageClass
                    {
                        ID = id,
                        Name = act
                    };
                    usageTable.Insert(usage);
                    Console.WriteLine("下記をUsagesTableに追加しました");
                    Console.WriteLine("     ---------- ----------     ");
                    Console.WriteLine("   ID   : {0} \r\n Action : {1}", usage.ID, usage.Name);
                    Console.WriteLine("     ---------- ----------     \r\n \r\n");
                }


                var actlog = usageTable.FindOne(x => x.Name.Contains(act));
                UsageLog log = new UsageLog
                {
                    ID = new Guid(),
                    UserID = userId,
                    UsageID = actlog.ID,
                    Date = date
                };
                usageLogTable.Insert(log);
                Console.WriteLine("下記をUsageLogTableに追加しました。");
                Console.WriteLine("     ---------- ----------     ");
                Console.WriteLine("   ID   : " + log.ID);
                Console.WriteLine(" UserID : " + log.UserID);
                Console.WriteLine("UsageID : " + log.UsageID);
                Console.WriteLine("  Date  : " + log.Date);
                Console.WriteLine("     ---------- ----------     \r\n \r\n");
            }
        }

      
    }
}