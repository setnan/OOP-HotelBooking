import { useState } from "react";
import Head from "next/head";
import styles from "../styles/Login.module.css";
import { useRouter } from "next/router";

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
      const response = await fetch("http://localhost:5000/api/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password }),
      });

      if (!response.ok) {
        const errorData = await response.text();
        setError(errorData || "Feil brukernavn eller passord");
        setIsLoading(false);
        return;
      }

      const data = await response.json();

      // Eksempel: data = { token: "...", role: "admin" }
      localStorage.setItem("token", data.token);
      localStorage.setItem("role", data.role);

      router.push("/dashboard");
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
              placeholder="Brukernavn"
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
