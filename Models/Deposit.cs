using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MetasAhorro.Models
{
    public class Deposit
    {
        public int Id { get; set; }
        public int GoalId { get; set; }

        [ForeignKey(nameof(GoalId))]
        public Goal? Goal { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        public string? Note { get; set; }
    }
}
