namespace Application.Features.OrderInstructions.Commands.ChangeOrderInstructionStatus
{
    public class ChangeOrderInstructionStatusDto
    {
        public int OrderInstructionId { get; set; }
        public bool IsActive { get; set; }
    }
}
