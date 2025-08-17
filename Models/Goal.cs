using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore; // <- necesario para [Index]

namespace MetasAhorro.Models
{
    [Index(nameof(OwnerKey))] // índice para filtrar por cookie
    public class Goal
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Title { get; set; } = "";

        [StringLength(200)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TargetAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal InitialAmount { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; } = DateTime.Today.AddMonths(6);

        // Dueño anónimo por cookie
        [Required, StringLength(64)]
        public string OwnerKey { get; set; } = "";

        public ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

        public decimal CurrentAmount => InitialAmount + (Deposits?.Sum(d => d.Amount) ?? 0m);
        public decimal Remaining     => Math.Max(0, TargetAmount - CurrentAmount);
        public double  ProgressPct   => TargetAmount <= 0 ? 0 :
            Math.Clamp((double)(CurrentAmount / TargetAmount) * 100.0, 0, 100);

        public int TotalMonths
        {
            get
            {
                var months = (Deadline.Year - StartDate.Year) * 12 + (Deadline.Month - StartDate.Month);
                return Math.Max(1, months <= 0 ? 1 : months);
            }
        }

        public decimal MonthlyNeeded
        {
            get
            {
                var today = DateTime.Today;
                var monthsLeft = (Deadline.Year - today.Year) * 12 + (Deadline.Month - today.Month);
                monthsLeft = Math.Max(1, monthsLeft);
                return Math.Ceiling(Remaining / monthsLeft);
            }
        }
    }
}
