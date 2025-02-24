import { Box, Container, CssBaseline } from "@mui/material";
import Navbar from "./NavBar";
import { Outlet } from "react-router";

function App() {
  return (
    <Box sx={{ bgcolor: '#eeeeee', minHeight: '100vh' }}>
      <CssBaseline />
      <Navbar />
      <Container maxWidth='xl' sx={{ mt: 3 }}>
        <Outlet /> {/*What this means is: When we [Browse] to a [Specific] [Route] 
                      Then the [Component] that we going to [Route to] 
                      Is going to [Replace] This [<Outlet />] */}
      </Container>
    </Box>
  )
}

export default App
