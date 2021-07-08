﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Phenryr.Database
{
    public partial class EightBallAnswer
    {
        [Key]
        public long AnswerId { get; set; }
        public string AnswerText { get; set; }
        public string AnswerColor { get; set; }

    }
}
