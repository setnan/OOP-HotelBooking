import "../styles/styles.css";
import Head from "next/head";
import { AuthProvider } from "../context/AuthContext";

export default function MyApp({ Component, pageProps }) {
  return (
    <AuthProvider>
      <Head>
        <title>Hotel Booking App</title>
        <link rel="icon" href="/favicon.png" type="image/png" />
      </Head>
      <Component {...pageProps} />
    </AuthProvider>
  );
}
