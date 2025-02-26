import { Box, Container, CssBaseline } from "@mui/material";
import Navbar from "./NavBar";
import { Outlet, useLocation } from "react-router";
import HomePage from "../../features/home/HomePage";

function App() {

  const location = useLocation();

  return (
    <Box sx={{ bgcolor: '#eeeeee', minHeight: '100vh' }}>
      <CssBaseline />
      {location.pathname === '/' ? <HomePage /> : (
        <>
          <Navbar />
          <Container maxWidth='xl' sx={{ mt: 3 }}>
            <Outlet /> {/*What this means is: When we [Browse] to a [Specific] [Route] 
                      Then the [Component] that we going to [Route to] 
                      Is going to [Replace] This [<Outlet />] */}
          </Container>
        </>
      )}
    </Box>
  )
}

export default App
