import { useEffect, useState } from "react";
import Head from "next/head";
import styles from "../styles/Home.module.css";
import PrivateRoute from "../components/PrivateRoute";
import { useAuth } from "../context/AuthContext";

export default function AdminPage() {
  const { logout } = useAuth();
  const [users, setUsers] = useState([]);
  const [menuOpen, setMenuOpen] = useState(false);

  useEffect(() => {
    // Henter alle brukere fra backend
    fetch("http://localhost:5127/api/users")
      .then((res) => res.json())
      .then(setUsers)
      .catch((err) => console.error("Feil ved henting av brukere:", err));
  }, []);

  return (
    <PrivateRoute>
      <Head>
        <title>Admin - Gokstad Hotel</title>
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
              <li>Brukerstyring</li>
              <li>Romadministrasjon</li>
              <li>Se alle bookinger</li>
              <li onClick={logout} style={{ cursor: "pointer", color: "#c00" }}>
                Logg ut
              </li>
            </ul>
          </nav>
        )}
      </header>

      <main className={styles.main}>
        <h1>Adminpanel</h1>
        <h2>Brukere</h2>
        {users.length > 0 ? (
          <ul>
            {users.map((user) => (
              <li key={user.userId}>
                {user.name} ({user.role}) – {user.email}
              </li>
            ))}
          </ul>
        ) : (
          <p>Ingen brukere funnet.</p>
        )}
      </main>
    </PrivateRoute>
  );
}
