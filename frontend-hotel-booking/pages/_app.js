import "../styles/styles.css";
import Head from "next/head";
import { AuthProvider } from "../context/AuthContext";
import { useEffect } from "react";
import { useRouter } from "next/router";

export default function MyApp({ Component, pageProps }) {
  const router = useRouter();

  useEffect(() => {
    const protectedRoutes = ["/receptionist", "/admin"];
    const isProtected = protectedRoutes.includes(router.pathname);

    const role =
      typeof window !== "undefined" ? localStorage.getItem("role") : null;
    console.log("JAVEEEL");
    if (isProtected && !role) {
      router.push("/login");
    }
  }, [router]);

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
