import { useState } from "react";
import Head from "next/head";
import styles from "../styles/Login.module.css";
import { useRouter } from "next/router";
import { api } from "../services/api";

export default function LoginPage() {
  const router = useRouter();

  const [forgotPassword, setForgotPassword] = useState(false);
  const [email, setEmail] = useState("");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const handleForgotPassword = () => {
    setForgotPassword(true);
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    setError("");
    setIsLoading(true);

    try {
      const data = await api.post("/api/login", { username, password });

      // Store the token first
      if (data.token) {
        localStorage.setItem("token", data.token);
      }

      // Then store user data
      localStorage.setItem("userId", data.userId);
      localStorage.setItem("name", data.name);
      localStorage.setItem("email", data.email);
      localStorage.setItem("role", data.role);

      // Navigate based on role
      if (data.role === "Admin") {
        router.push("/admin");
      } else if (data.role === "Receptionist") {
        router.push("/receptionist");
      } else {
        setError("Ukjent brukerrolle.");
      }
    } catch (err) {
      setError("Klarte ikke koble til serveren.");
    } finally {
      setIsLoading(false);
    }
  };

  const handleResetPassword = () => {
    alert(`Sender reset-link til ${email}`);
  };

  return (
    <>
      <Head>
        <title>Hotel Booking Innlogging</title>
      </Head>
      <div className={styles.heroImage}>
        <main className={styles.container}>
          <h1 className={styles.title}>Hotel Booking Login</h1>

          <form className={styles.form} onSubmit={handleLogin}>
            <input
              type="text"
              placeholder="Brukernavn (e-post)"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
            <input
              type="password"
              placeholder="Passord"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />

            <button type="submit" disabled={isLoading}>
              {isLoading ? "Logger inn..." : "Logg inn"}
            </button>

            {error && <p style={{ color: "red" }}>{error}</p>}

            {!forgotPassword && (
              <p className={styles.forgot} onClick={handleForgotPassword}>
                Glemt passord?
              </p>
            )}

            {forgotPassword && (
              <div className={styles.emailReset}>
                <p>Skriv inn e-post tilknyttet brukernavn:</p>
                <input
                  type="email"
                  placeholder="Din e-post"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                />
                <button type="button" onClick={handleResetPassword}>
                  Send tilbakestilling
                </button>
              </div>
            )}
          </form>
        </main>
      </div>
    </>
  );
}
