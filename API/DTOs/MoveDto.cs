using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Logic;

namespace API.DTOs
{
    public class MoveDto
    {
        [Required]
        public string gameId { get; set; }
        [Required]
        public Position From { get; set; }
        [Required]
        public Position To { get; set; }
    }
}