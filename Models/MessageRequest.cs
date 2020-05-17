using System.ComponentModel.DataAnnotations;

namespace cipher_server
{
    /// <summary>
    /// Submitted message from user
    /// </summary>
    public class MessageRequest
    {
        public int Shift { get; set; }
        [Required]
        public string Message { get; set; }
    }
}