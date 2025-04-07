import { useEffect } from "react";
import { useRouter } from "next/router";
import { useAuth } from "../context/AuthContext";

export default function PrivateRoute({ children }) {
  const router = useRouter();
  const { token, loading } = useAuth();

  useEffect(() => {
    if (!loading && !token) {
      router.push("/login");
    }
  }, [loading, token, router]);

  // Ikke vis noe mens vi sjekker om brukeren er logget inn
  if (loading) return null;

  // Bare render innholdet dersom brukeren er autentisert
  return token ? children : null;
}
