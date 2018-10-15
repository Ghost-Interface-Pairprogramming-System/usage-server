using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using LiteDB;

namespace GIPS.Controllers
{


    [RoutePrefix("api/v1")]
    public class FirstContactController : ApiController
    {
        readonly string FILE_NAME = HttpRuntime.AppDomainAppPath + "usages.db";
        readonly string FIRST_CONTACT = "FirstContact";

        // GET api/v1/FirstContact

        /// <summary>
        /// 初回起動時に使われる
        /// ユーザーIDを返すと同時に初回起動日をusageに保存する。
        /// </summary>
        /// <returns>Guid形式のuserid</returns>
        [Route("FirstContact")]
        [HttpGet]
        public string FirstContact()
        {


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




        //POST api/v1/Usage

        /// <summary>
        /// 送られてきたユーザーIDとactionをDBに保存する。
        /// </summary>
        /// <returns>UsageRequest型のrequest </returns>
        /// <param name="request">Request.</param>
        [Route("Usage")]
        [HttpPost]
        public UsageRequest Usage(UsageRequest request)
        {
            string action = request.RequestAction;
            DateTime date = request.Date;
            AddUsageLog(Guid.Parse(request.UserID), date, action);
            return request;
        }

        /// <summary>
        /// Usageで一回一回送れなかった場合にまとめてDBに保存するために使う。
        /// </summary>
        /// <param name="request">Request.</param>
        // POST api/v1/Usages
        [Route("Usages")]
        [HttpPost]
        public void Usages(UsagesRequest request)
        {

            Guid userId = Guid.Parse(request.UserId);

            foreach (Usage usage in request.Usages)
            {
                AddUsageLog(userId,usage.Date,usage.Action);
            }

        }

        [Route("Usage/List")]
        [HttpGet]
        public UsageLog[] UsageList()
        {
            using (var db = new LiteDatabase(FILE_NAME))
            {
                return db.GetCollection<UsageLog>("UsageLogs").FindAll().ToArray();
            }            
        }

        //ここから関数
        /// <summary>
        /// userIDをTableに追加する関数
        /// </summary>
        /// <param name="userId">User identifier.</param>
        private void AddUser(Guid userId)
        {
            using (var db = new LiteDatabase(FILE_NAME))
            {
                var usertable = db.GetCollection<UserClass>("Users");
                UserClass user = new UserClass { ID = userId };
                usertable.Insert(user);
            }
        }

        /// <summary>
        /// UsageLogTableにログを追加するための関数
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="date">行動が実行された時間</param>
        /// <param name="act">行動を表す文字列</param>
        private void AddUsageLog(Guid userId, DateTime date,string act)
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



                    Debug.WriteLine("下記をUsagesTableに追加しました");
                    Debug.WriteLine("     ---------- ----------     ");
                    Debug.WriteLine("   ID   : {0} \r\n Action : {1}", usage.ID, usage.Name);
                    Debug.WriteLine("     ---------- ----------     \r\n \r\n");
                }


                var actlog = usageTable.FindOne(x => x.Name.Contains(act));
                UsageLog log = new UsageLog
                {
                    ID = Guid.NewGuid(),
                    UserID = userId,
                    UsageID = actlog.ID,
                    Date = date
                };
                usageLogTable.Insert(log);
                Debug.WriteLine("下記をUsageLogTableに追加しました。");
                Debug.WriteLine("     ---------- ----------     ");
                Debug.WriteLine("   ID   : " + log.ID);
                Debug.WriteLine(" UserID : " + log.UserID);
                Debug.WriteLine("UsageID : " + log.UsageID);
                Debug.WriteLine("  Date  : " + log.Date);
                Debug.WriteLine("     ---------- ----------     \r\n \r\n");
            }
        }

      
    }
}