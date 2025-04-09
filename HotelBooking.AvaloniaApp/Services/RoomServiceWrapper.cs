using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.Services;

public class RoomServiceWrapper
{
    private readonly RoomService roomService;

    public RoomServiceWrapper(RoomService roomService)
    {
        this.roomService = roomService;
    }

    public async Task<IEnumerable<Room>> GetAllRoomsAsync()
    {
        return await Task.FromResult(roomService.GetAllRooms());
    }

    public async Task<Room?> GetRoomAsync(int id)
    {
        return await Task.FromResult(roomService.GetRoom(id));
    }

    public async Task<Room> CreateRoomAsync(Room room)
    {
        return await Task.FromResult(roomService.CreateRoom(room));
    }

    public async Task<Room> UpdateRoomAsync(Room room)
    {
        return await Task.FromResult(roomService.UpdateRoom(room));
    }

    public async Task DeleteRoomAsync(int id)
    {
        await Task.Run(() => roomService.DeleteRoom(id));
    }

    public async Task<int> GetTotalRoomsAsync()
    {
        return await Task.FromResult(roomService.GetTotalRooms());
    }

    public async Task<decimal> GetOccupancyRateAsync()
    {
        return await Task.FromResult(roomService.GetOccupancyRate());
    }

    public async Task<IEnumerable<RoomUsageStats>> GetRoomUsageStatsAsync()
    {
        return await Task.FromResult(roomService.GetRoomUsageStats());
    }
}
