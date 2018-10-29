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
            <div class="col-12" style="background-color:brown">
                <canvas id="Graph" height="300" width="500"></canvas>
            </div>
        </div>
    </div>
</body>
    <script>
        var ctx = document.getElementById("Graph").getContext("2d");
        
        var userid = "8c80e8e1-a4b7-4338-8c06-b13029726b0c"
        var logs = {};

        $.ajax(
        {
            url : '/api/v1/ActionLog', 
            type : 'GET',
            dataType : "json",
            data : 
            {
                'userid' : userid
            }
        })
        .done((data) => 
        {
            $.each(data,function(id,val)
            {
                var date = (new Date().getDate() - id).toString();
                logs.date = val;
                console.log("while   : " ,logs[date]);
            });
            console.log(logs);
            // var chart = new Chart(ctx,
            // {
            //     type : 'line',
            //     data : 
            //     {

            //     }
            // });
        })
        .fail( (data) => 
        {
            $('.result').html(data);
            console.log(data);
        });
        

    </script>
</html>
