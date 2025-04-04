import { useEffect, useState } from "react";
import Head from "next/head";
import styles from "../styles/Home.module.css";

export default function Home() {
  const [rooms, setRooms] = useState([]);
  const [selectedRoomId, setSelectedRoomId] = useState("");
  const [checkInDate, setCheckInDate] = useState("");
  const [checkOutDate, setCheckOutDate] = useState("");
  const [menuOpen, setMenuOpen] = useState(false);

  useEffect(() => {
    // Dummy data til backend er koblet på
    setRooms([
      { id: 1, name: "Rom 101" },
      { id: 2, name: "Rom 202" },
      { id: 3, name: "Rom 303" },
    ]);
  }, []);

  const handleSubmit = (e) => {
    e.preventDefault();

    const selectedRoom = rooms.find((r) => r.id.toString() === selectedRoomId);

    alert(
      `Booking:
      Navn: ${e.target[0].value} ${e.target[1].value}
      Rom: ${selectedRoom?.name || selectedRoomId}
      Fra: ${checkInDate}
      Til: ${checkOutDate}`
    );
  };

  return (
    <>
      <Head>
        <title>Hotel Booking</title>
      </Head>

      <header className={styles.navbar}>
        <div
          className={`${styles.menuIcon} ${menuOpen ? styles.open : ""}`}
          onClick={() => setMenuOpen(!menuOpen)}
        >
          ☰
        </div>

        {menuOpen && (
          <nav className={styles.sidebar}>
            <ul>
              <li>Se alle bookinger</li>
              <li>Romoversikt</li>
              <li>Innstillinger</li>
              <li>Logg ut</li>
            </ul>
          </nav>
        )}

        <input
          type="text"
          placeholder="Søk i databasen..."
          className={styles.search}
        />
      </header>

      <main className={styles.main}>
        <h1>Book a Room</h1>

        <form className={styles.form} onSubmit={handleSubmit}>
          <input type="text" placeholder="Navn" required />
          <input type="text" placeholder="Etternavn" required />

          <select
            required
            className={styles.select}
            value={selectedRoomId}
            onChange={(e) => setSelectedRoomId(e.target.value)}
          >
            <option value="">Velg et rom</option>
            {rooms.map((room) => (
              <option key={room.id} value={room.id}>
                {room.name || `Rom ${room.id}`}
              </option>
            ))}
          </select>

          <input
            type="date"
            value={checkInDate}
            onChange={(e) => setCheckInDate(e.target.value)}
            required
          />
          <input
            type="date"
            value={checkOutDate}
            onChange={(e) => setCheckOutDate(e.target.value)}
            required
          />

          <button type="submit">Book</button>
        </form>
      </main>
    </>
  );
}
