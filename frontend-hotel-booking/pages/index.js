import { useEffect } from "react";
import { useRouter } from "next/router";
import { useAuth } from "../context/AuthContext";

export default function Home() {
  const { token, loading } = useAuth();
  const router = useRouter();

  useEffect(() => {
    if (!loading) {
      router.replace(token ? "/dashboard" : "/login");
    }
  }, [token, loading]);

  return <p>Laster...</p>;
}
