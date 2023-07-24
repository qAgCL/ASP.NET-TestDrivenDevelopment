using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomBookingController : ControllerBase
{
    private readonly IRoomBookingRequestProcessor _roomBookingProcessor;

    public RoomBookingController(IRoomBookingRequestProcessor roomBookingProcessor)
    {
        this._roomBookingProcessor = roomBookingProcessor;
    }

    [HttpPost]
    public Task<IActionResult> BookRoom(RoomBookingRequest request)
    {
        if (ModelState.IsValid)
        {
            RoomBookingResult? result = _roomBookingProcessor.BookRoom(request);
            if (result.Flag == Core.Enums.BookingResultFlag.Success)
            {
                return Task.FromResult<IActionResult>(Ok(result));
            }

            ModelState.AddModelError(nameof(RoomBookingRequest.Date), "No Rooms Available For Given Date");
        }

        return Task.FromResult<IActionResult>(BadRequest(ModelState));
    }
}
