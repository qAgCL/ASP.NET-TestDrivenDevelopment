﻿using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;

namespace RoomBookingApp.Core.Processors;

public class RoomBookingRequestProcessor
{
    private readonly IRoomBookingService _roomBookingService;

    public RoomBookingRequestProcessor(IRoomBookingService roomBookingService)
    {
        _roomBookingService = roomBookingService;
    }

    public RoomBookingResult BookRoom(RoomBookingRequest bookingRequest)
    {
        if (bookingRequest is null)
        {
            throw new ArgumentNullException(nameof(bookingRequest));
        }

        var availableRooms = _roomBookingService.GetAvailableRooms(bookingRequest.Date).ToList();
        var result = CreateRoomBookingObject<RoomBookingResult>(bookingRequest);

        if (availableRooms.Any())
        {
            Room room = availableRooms.First();
            var roomBooking = CreateRoomBookingObject<RoomBooking>(bookingRequest);
            roomBooking.RoomId = room.Id;
            _roomBookingService.Save(roomBooking);

            result.RoomBookingId = roomBooking.Id;
            result.Flag = BookingResultFlag.Success;
        }
        else
        {
            result.Flag = BookingResultFlag.Failure;
        }

        return result;
    }

    private static TRoomBooking CreateRoomBookingObject<TRoomBooking>(RoomBookingRequest bookingRequest) where TRoomBooking
        : RoomBookingBase, new()
    {
        return new TRoomBooking
        {
            FullName = bookingRequest.FullName,
            Date = bookingRequest.Date,
            Email = bookingRequest.Email,
        };
    }
}