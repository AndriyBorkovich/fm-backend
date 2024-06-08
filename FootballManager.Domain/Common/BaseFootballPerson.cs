using System.ComponentModel.DataAnnotations;

namespace FootballManager.Domain.Common;

public class BaseFootballPerson : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; }
    [StringLength(100)]
    public string Nationality { get; set; }
    public DateTime BirthDay { get; set; }

    public int CalculateAge()
    {
        var currentDate = DateTime.Today;

        var age = currentDate.Year - this.BirthDay.Year;

        // Check if the birthday hasn't occurred yet this year
        if (currentDate.Month < this.BirthDay.Month ||
            (currentDate.Month == this.BirthDay.Month && currentDate.Day < this.BirthDay.Day))
        {
            age--;
        }

        return age;
    }
}
