<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginPage.aspx.cs" Inherits="LoginPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Login page</title>
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.10.1/dist/sweetalert2.all.min.js"></script>
    <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/sweetalert2@10.10.1/dist/sweetalert2.min.css' />
    <script src="//netdna.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
    <script>
        function HideLabe(msg) {
            Swal.fire({
                icon: 'success',
                text: msg,
                timer: 1000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
            window.location.href ="../Admin/Dashboard.aspx";
            })
        };
    </script>
    <script>
        function HideLabel(msg) {
            debugger;
            Swal.fire({
                icon: 'error',
                text: msg,
                timer: 4000,
                showCancelButton: false,
                showConfirmButton: false
            }).then(function () {
                //window.location.href = "";
            })
        };
    </script>
    <style>
        .backg {
            background-image: url(image/Final.jpg);
            background-position: center;
            background-repeat: no-repeat;
            background-size: cover;
            width: 100%;
        }

        .Loginbox {
            margin-top: 0px;
            /* background-color:#3151CB;*/
        }

        .panel-transparent {
            background: none;
            background: rgb(2,0,36);
            background: linear-gradient(9deg, rgba(2,0,36,1) 0%, rgba(18,37,199,1) 17%, rgba(9,9,121,1) 47%, rgba(14,23,160,1) 81%, rgba(0,212,255,1) 100%);
        }

        marquee {
            font-size: 20px;
            color: #163a8c;
            font-family: Tahoma, sans-serif;
        }

        .Header {
            background-color: white;
        }

        .colorIcon {
            color: #285e8e;
        }

        .weblink {
            font-size: 14px;
        }

        @media only screen and (max-width: 767px) {
            .backg {
    background-image: none;
    background-position: center;
    background-repeat: no-repeat;
    background-size: contain;
    width: 100%;
}

         
            .weblink {
            font-size: 14px;
            text-align:center;
        }
        }

        
        /*.body {
     font-family: "Open Sans", -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Oxygen-Sans, Ubuntu, Cantarell, "Helvetica Neue", Helvetica, Arial, sans-serif; 
}*/
    </style>
</head>

<body class="backg">

    <form id="form1" runat="server">

        <div class="container-fluid" style="margin-top: 20px">
            <div class="col-md-4 Loginbox">
                <div class=" panel panel-default panel-transparent">
                    <div class="Header">
                        <hr />
                        <center><img src="image/NewLogo.png" class="img-responsive" style="width: 150px; height: 120px" /></center>
                        <hr />
                    </div>
                    <div class="panel-body">
                        <fieldset>
                            <div class="row">
                                <div class="center-block" style="margin-top: -20px">
                                    <center><img src="image/user-login.png" style="border-radius:60px" alt="" class="profile-img small" /></center>
                                </div>
                                <marquee class="mt-3" style="color: floralwhite"><b>Welcome To Endeavour Automation</b></marquee>
                            </div>
                            <br />
                            <div class="col-sm-12 col-md-10  col-md-offset-1 top">
                                <div class="form-group">
                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            <i class="glyphicon glyphicon-user colorIcon"></i>
                                        </span>
                                        <asp:TextBox ID="txtusername" runat="server" class="form-control" placeholder="User name" autofocus="true" required="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            <i class="glyphicon glyphicon-lock colorIcon"></i>
                                        </span>
                                        <asp:TextBox ID="txtpassword" runat="server" class="form-control" placeholder="Password" TextMode="Password" required="true"></asp:TextBox>
                                    </div>
                                    <asp:Label runat="server" ID="lbl" ForeColor="red"></asp:Label>
                                </div>
                                <div class="form-group">
                                    <center></center>
                                    <asp:Button ID="submit" runat="server" Text="Sign in" OnClick="submit_Click" class="  btn btn-lg btn-primary btn-block " data-toggle="modal" data-target="#centralModalSm" />
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div class="weblink">
                    <b>Designed & Developed By
                    <a target="_blank" style="color: red" href="http://weblinkservices.net/"><span>Web Link Services Pvt. Ltd.</span></a></b>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

