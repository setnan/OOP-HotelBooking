import { useEffect } from 'react';
import { useRouter } from 'next/router';
import { useAuth } from '../context/AuthContext';

export default function PrivateRoute({ children }) {
  const router = useRouter();
  const { token, loading } = useAuth();

  useEffect(() => {
    if (!loading && !token) {
      router.push('/login');
    }
  }, [loading, token, router]);

  // Don't render anything while checking authentication
  if (loading) return null;

  // Only render children if authenticated
  return token ? children : null;
}
