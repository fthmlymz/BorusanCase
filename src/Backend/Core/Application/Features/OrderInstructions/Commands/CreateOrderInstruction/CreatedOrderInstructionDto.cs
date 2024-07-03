namespace Application.Features.OrderInstructions.Commands.CreateOrderInstruction
{
    public class CreatedOrderInstructionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DayOfMonth { get; set; }
        public decimal Amount { get; set; }
        public List<NotificationChannelDto>? NotificationChannels { get; set; }
    }
}
