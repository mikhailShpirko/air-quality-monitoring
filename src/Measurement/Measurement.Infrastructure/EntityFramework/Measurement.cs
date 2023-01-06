using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Measurement.Infrastructure.EntityFramework
{
    [Table("measurements")]
    internal sealed class Measurement
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("pm2_5")]
        public decimal PM2_5 { get; set; }

        [Required]
        [Column("pm10")]
        public decimal PM10 { get; set; }

        [Required]
        [Column("timestampticks")]
        public long TimestampTicks { get; set; }
    }
}
