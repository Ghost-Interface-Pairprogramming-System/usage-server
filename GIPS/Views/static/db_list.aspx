﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css" integrity="sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO" crossorigin="anonymous">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <title>DB-List</title>
</head>
    <body>
    <div class="container">
       <div class="row justify-content-between">
           <table class="table">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>UserID</th>
                        <th>Action</th>
                        <th>Date</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
           </table>
       </div>
    </div>
    <script>
        $(function()
        {
            $.ajax(
            {
                // url : 'http://127.0.0.1:8080/api/v1/Usage/List',
                url : '/api/v1/Usage/List', 
                type : 'GET',
                dataType : "json"
            })
            .done( (data) =>
            {
                var action;

                $.each(data['Item1'],function(id,list)
                {
                    console.log(list);
                    $.each(data['Item2'],function(actid,act)
                    {
                        if(list['UsageID'] === act['ID'])action = act['Name'];
                        console.log(action);
                    });
                    $('tbody').append
                    (
                        $('<tr></tr>')
                        .append($('<td></td>')).text(id)
                        .append($('<td></td>').text(list.UserID))
                        .append($('<td></td>').text(action))
                        .append($('<td></td>').text(list.Date))
                    )
                });
            })
            .fail( (data) =>
            {
                console.log(data);
            })
            .always( (data) =>
            {

            });
        });
    </script>
</body>
</html>
