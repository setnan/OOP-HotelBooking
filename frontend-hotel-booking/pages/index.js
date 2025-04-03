import Head from "next/head";
import "../styles/styles.css";

export default function Home() {
  return (
    <>
      <Head>
        <title>Hotel Booking</title>
        <script src="/script.js" defer></script>
      </Head>

      <main>
        <h1>Book a Room</h1>

        <form id="bookingForm">
          <input type="number" id="guestId" placeholder="Guest ID" required />
          <input type="number" id="roomId" placeholder="Room ID" required />
          <input type="date" id="checkIn" required />
          <input type="date" id="checkOut" required />
          <button type="submit">Book</button>
        </form>

        <h2>Existing Bookings</h2>
        <ul id="bookingList"></ul>
      </main>
    </>
  );
}
