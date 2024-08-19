var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapControllers();


string htmlText = @$"
<!doctype html>
<html>
    <head>
        <title>Ebenin Queue</title>
        <meta charset=""utf-8""><meta name=""viewport"" content=""width=device-width, initial-scale=1"">
        <link rel=""shortcut icon"" href=""#"">
        <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css"" rel=""stylesheet"">
        <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js""></script> 

        <style>
            html {{color:black}}
        </style>

    </head>
    
    <body class=""container pt-3"">
        <h5>Channel List</h5>

        <div>
            Memory : <div class=""d-inline"" id=""divMemory""></div>
        <div>

        <div id=""divTable""></div>        

    </body>
</html>

<script>
    document.addEventListener(""DOMContentLoaded"", function() {{
        setInterval(function () {{
            fetch('QueuesApi/GetMemory')
            .then(response => response.text())
            .then((data) =>  {{
                document.querySelector('#divMemory').innerHTML=data;
            }} );
        }}, 2000);

        setInterval(function () {{
            fetch('QueuesApi/ChannelInfo')
            .then(response => response.text())
            .then((data) =>  {{
                document.querySelector('#divTable').innerHTML=data;
            }} );
        }}, 2000);
    }});
</script>

";

app.MapGet("/", () => Results.Content(htmlText, Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/html")));


app.Run();
