document.addEventListener("DOMContentLoaded", () => {
  const form = document.getElementById("bookingForm");
  const list = document.getElementById("bookingList");

  // Fetch bookings from backend
  /*   fetch("https://your-backend-api-url/booking")
    .then((res) => res.json())
    .then((bookings) => {
      bookings.forEach((b) => {
        const li = document.createElement("li");
        li.textContent = `Room ${b.room.roomNumber} - Guest: ${b.guest.name} (${b.checkIn} to ${b.checkOut})`;
        list.appendChild(li);
      });
    }); */

  // Submit form
  form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const booking = {
      guest: { guestId: parseInt(document.getElementById("guestId").value) },
      room: { roomId: parseInt(document.getElementById("roomId").value) },
      checkIn: document.getElementById("checkIn").value,
      checkOut: document.getElementById("checkOut").value,
    };

    const res = await fetch("https://your-backend-api-url/booking", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(booking),
    });

    if (res.ok) {
      alert("Booking registered!");
      window.location.reload();
    } else {
      alert("Something went wrong.");
    }
  });
});
