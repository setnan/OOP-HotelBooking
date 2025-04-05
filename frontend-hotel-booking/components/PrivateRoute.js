import PrivateRoute from "../components/PrivateRoute";

export default function Dashboard() {
  return (
    <PrivateRoute>
      <h1>Velkommen til Dashboardet</h1>
    </PrivateRoute>
  );
}
