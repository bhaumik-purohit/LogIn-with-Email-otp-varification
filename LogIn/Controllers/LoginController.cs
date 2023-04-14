using LogIn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using NuGet.Protocol.Plugins;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Xml.Linq;



namespace LogIn.Controllers
{
    public class LoginController : Controller
    {
        private string errorMessage;
        private string successMessage;

        public IActionResult Otp()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Otp(Onetimepass iList)
        {
           string otp =  HttpContext.Session.GetString("otp");
            if (iList.otp == otp)
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                string error = "OTP DOES NOT MATCH!";
                ViewBag.error = error;
                return RedirectToAction("Otp");
            }
           
        }
        

        public IActionResult Dashboard()
        {
            int idn = (int)HttpContext.Session.GetInt32("id");
            ViewBag.id = idn;
            string u_name = HttpContext.Session.GetString("u_name");
            ViewBag.u_name = u_name;
            string user_name = HttpContext.Session.GetString("user_name");
            ViewBag.user_name = user_name;
            string email = HttpContext.Session.GetString("email");
            ViewBag.email = email;
            string mobile_number = HttpContext.Session.GetString("mobile_number");
            ViewBag.mobile_number = mobile_number;
            string date_of_birth = HttpContext.Session.GetString("date_of_birth");
            ViewBag.date_of_birth = date_of_birth;
            string gender = HttpContext.Session.GetString("gender");
            ViewBag.gender = gender;
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Register iList)
        {
            try
            {
                string con = "server=DESKTOP-S2MQOT2;database=user_data;Trusted_connection=true;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    string query = "select * from register where username='"+ iList.user_name +"' and pass_word='"+ iList.pass_word +"'";
                    SqlCommand cmd = new SqlCommand(query,connection);

                    cmd.Parameters.AddWithValue("@username", iList.user_name);
                    cmd.Parameters.AddWithValue("@pass_word", iList.pass_word);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        iList.Id = (int)reader["id"];
                        iList.u_name = reader["u_name"].ToString();
                        iList.email = reader["email"].ToString();
                        iList.mobile_number = reader["mobile_number"].ToString();
                        iList.date_of_birth = (DateTime)reader["date_of_birth"];
                        iList.gender = reader["gender"].ToString();
                         
                    }


                    if (reader.HasRows)
                    {
                        string dob = iList.date_of_birth.ToString();


                        HttpContext.Session.SetInt32("id", iList.Id);
                        HttpContext.Session.SetString("u_name", iList.u_name);
                        HttpContext.Session.SetString("user_name", iList.user_name);
                        HttpContext.Session.SetString ("email", iList.email);
                        HttpContext.Session.SetString("mobile_number", iList.mobile_number);
                        HttpContext.Session.SetString("date_of_birth", dob);
                        HttpContext.Session.SetString("gender", iList.gender);


                        Random rand = new Random();
                        string randomCode = (rand.Next(999999)).ToString();
                        HttpContext.Session.SetString("OTP", randomCode);

                        MailMessage msg = new MailMessage();

                        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                        smtpServer.Credentials = new System.Net.NetworkCredential("purohitbhaumik3143@gmail.com", "isfiqbggioknndmn");
                        smtpServer.Port = 587;
                        smtpServer.EnableSsl = true;

                        msg.From = new MailAddress("purohitbhaumik3143@gmail.com");
                        msg.To.Add(iList.email);
                        msg.Subject = "OTP";
                        msg.Body =  randomCode;

                        smtpServer.Send(msg);

                       return RedirectToAction("Otp");
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        


        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(Register register)
        
        
        {

            try
            {
                var friends = string.Empty;
                foreach (var item in register.friends)
                {
                    friends += item + ",";
                }

                string con = "server=DESKTOP-S2MQOT2;database=user_data;Trusted_connection=true;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    String sql = "insert into register(u_name,username,pass_word,confirm_password,email,mobile_number,date_of_birth,gender,friends) Values('" + register.u_name + "','" + register.user_name + "','" + register.pass_word + "','" + register.confirm_password + "','" + register.email + "','" + register.mobile_number + "','" + register.date_of_birth + "','" + register.gender + "','" + friends + "')";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                       //string dateob = register.date_of_birth.ToString();

                        command.Parameters.AddWithValue("@u_name", register.u_name);
                        command.Parameters.AddWithValue("@user_name", register.user_name);
                        command.Parameters.AddWithValue("@pass_word", register.pass_word);
                        command.Parameters.AddWithValue("@conform_pass", register.confirm_password);
                        command.Parameters.AddWithValue("@email", register.email);
                        command.Parameters.AddWithValue("@mobile_number", register.mobile_number);
                        command.Parameters.AddWithValue("@date_of_birth", register.date_of_birth);
                        command.Parameters.AddWithValue("@gender", register.gender);
                        command.Parameters.AddWithValue("@friends", friends);
                       

                        command.ExecuteNonQuery();
                    }
                }
            }


            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return View();
            }
            successMessage = "New client Added Correctly";
            Response.Redirect("/Login/Register");
            return View();
        }

       





    }

}
            
            
        

   

