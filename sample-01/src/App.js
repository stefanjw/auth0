import React, { useEffect, useState } from "react";
import { Router, Route, Switch } from "react-router-dom";
import { Container } from "reactstrap";

import Loading from "./components/Loading";
import NavBar from "./components/NavBar";
import Footer from "./components/Footer";
import Home from "./views/Home";
import Profile from "./views/Profile";
import ExternalApi from "./views/ExternalApi";
import { useAuth0 } from "@auth0/auth0-react";
import history from "./utils/history";

// styles
import "./App.css";

// fontawesome
import initFontAwesome from "./utils/initFontAwesome";
initFontAwesome();

const App = () => {
  const { isLoading, error, isAuthenticated, user, getAccessTokenSilently } = useAuth0(); // getAccessTokenSilently is still here
  const [userMetadata, setUserMetadata] = useState(null);
  
  useEffect(() => {
    
    const sendUserDataToBackend = async () => {
      const domain = "dev-nb826bcawvcdo07n.us.auth0.com";
        try {
          
           const token = await getAccessTokenSilently({
             authorizationParams: {
              audience: "https://dev-nb826bcawvcdo07n.us.auth0.com/api/v2/", 
               scope: "update:data", 
             },
          });

          // Prepare user data payload
          const userProfile = {
            userId: user.sub, // unique identifier from Auth0
            email: user.email,
            name: user.name,
          };

          // Send the user data along with the token to the backend
          const response = await fetch("http://localhost:5275/api/user/save-profile", {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
               Authorization: `Bearer ${token}`, // Include the access token in the Authorization header
            },
            body: JSON.stringify(userProfile), // Send user profile data in the request body
          });

          const data = await response.json();
          console.log("Response from backend:", data);
        } catch (err) {
          console.error("Error sending user data to backend:", err);
        }
      
    };

    sendUserDataToBackend();
  }, [isAuthenticated, user, getAccessTokenSilently]);

  if (error) {
    return <div>Oops... {error.message}</div>;
  }

  if (isLoading) {
    return <Loading />;
  }

  return (
    <Router history={history}>
      <div id="app" className="d-flex flex-column h-100">
        <NavBar />
        <Container className="flex-grow-1 mt-5">
          <Switch>
            <Route path="/" exact component={Home} />
            <Route path="/profile" component={Profile} />
            <Route path="/external-api" component={ExternalApi} />
          </Switch>

          {/* Display user information for testing */}
          {isAuthenticated && (
            <div style={{ marginTop: "20px" }}>
              <h3>User Information:</h3>
              <p><strong>Name:</strong> {user.name}</p>
              <p><strong>Email:</strong> {user.email}</p>
              <p><strong>Sub (User ID):</strong> {user.sub}</p>
              {userMetadata ? (
                <pre>{JSON.stringify(userMetadata, null, 2)}</pre>
              ) : (
                "No user metadata defined"
              )}
            </div>
          )}
        </Container>
        <Footer />
      </div>
    </Router>
  );
};

export default App;







