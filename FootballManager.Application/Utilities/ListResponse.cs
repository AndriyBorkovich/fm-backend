using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Application.Utilities
{
    public class ListResponse<TResult>(List<TResult> result, int total)
    {
        [Required]
        public List<TResult> Result { get; set; } = result;
        [Required]
        public int Total { get; set; } = total;
    }
}
