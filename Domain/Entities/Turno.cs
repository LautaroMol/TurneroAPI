using System.ComponentModel.DataAnnotations.Schema;
using TurneroAPI.Domain.Enums;

namespace TurneroAPI.Domain.Entities
{
    public class Turno : EntityBase
    {
        // Foreign key for the Patient (User)
        public Guid PatientId { get; set; }

        // Foreign key for the Doctor (User)
        public Guid DoctorId { get; set; }

        /// <summary>
        /// The date and time of the appointment.
        /// </summary>
        public DateTime AppointmentDateTime { get; set; }

        /// <summary>
        /// Details provided by the patient, such as symptoms or reason for the visit.
        /// </summary>
        public string? Details { get; set; }

        /// <summary>
        /// The current status of the appointment (e.g., Programado, Confirmado, Cancelado).
        /// </summary>
        public TurnoStatus Status { get; set; }

        // Navigation properties

        [ForeignKey(nameof(PatientId))]
        public virtual User Patient { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public virtual User Doctor { get; set; }
    }
}
