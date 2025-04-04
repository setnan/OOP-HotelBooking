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
    alert("Logget inn (dummy)");
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
      </div>
    </>
  );
}
