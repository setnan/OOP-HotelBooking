import { useState } from "react";
import Head from "next/head";
import styles from "../styles/Login.module.css";

export default function LoginPage() {
  const [forgotPassword, setForgotPassword] = useState(false);
  const [email, setEmail] = useState("");

  const handleForgotPassword = () => {
    setForgotPassword(true);
  };

  const handleLogin = (e) => {
    e.preventDefault();
    // Her kan du legge til login-logikk senere
    alert("Logget inn (dummy)");
  };

  const handleResetPassword = () => {
    // Her kan du legge til logikk for å sende e-post hvis e-post stemmer
    alert(`Sender reset-link til ${email}`);
  };

  return (
    <>
      <Head>
        <title>Login</title>
      </Head>
      <main className={styles.container}>
        <h1>Hotel Booking Login</h1>

        <form className={styles.form} onSubmit={handleLogin}>
          <input type="text" placeholder="Brukernavn" required />
          <input type="password" placeholder="Passord" required />

          <button type="submit">Logg inn</button>

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
    </>
  );
}
