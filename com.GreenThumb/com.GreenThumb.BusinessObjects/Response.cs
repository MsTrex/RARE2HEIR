﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.GreenThumb.BusinessObjects
{
    public class Response
    {
        public int QuestionID { get; set; }
        public DateTime Date { get; set; }
        public string UserResponse { get; set; }
        public int UserID { get; set; }

        public Response() { }

        public Response(int questionID, DateTime date, string response, int userID)
        {
            QuestionID = questionID;
            Date = date;
            UserResponse = response;
            UserID = userID;
        }
    }
}