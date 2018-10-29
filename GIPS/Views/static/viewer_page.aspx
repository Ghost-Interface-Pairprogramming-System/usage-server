<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="UTF-8">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.3/Chart.bundle.min.js"></script>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO" crossorigin="anonymous">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <title>利用状況ページ</title>
</head>
<body>
    <div class="container">
        <div class="row">
            <div class="col-12 center h2">
                <p id = "Date"></p>
            </div>
        </div>
        <div class="row">
            <div class="col-12">
                <canvas id="Graph" height="300" width="500"></canvas>
            </div>
        </div>
    </div>
</body>
    <script>
        var today = new Date();
        $('#Date').text(today.getFullYear()+"年"+(today.getMonth()+1)+"月"+today.getDate()+"から過去７日間のAction情報");
        var ctx = document.getElementById("Graph").getContext("2d");
        var userid = "8c80e8e1-a4b7-4338-8c06-b13029726b0c";
        var dates = [];
        var logs = [];
        $.ajax(
        {
            url : '/api/v1/ActionLog',
            type : 'GET',
            dataType : "json",
            data : {'userid' : userid}
        })
        .done((data) =>
        {
            $.each(data,function(id,val)
            {
                var date = (new Date().getDate() - id).toString();
                dates[id] = date;
                logs[id] = val;
            });


            console.log(logs);



            var chart = new Chart(ctx,
            {
                type : 'bar',
                data :
                {
                    labels : dates.reverse(),
                    datasets:[
                        {
                            label : "Actionの数",
                            data : logs.reverse()
                        }]
                },
                options:{}
            });
        })
        .fail( (data) =>
        {
            $('.result').html(data);
            console.log(data);
        });

    </script>
</html>
