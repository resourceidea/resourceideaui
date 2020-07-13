import React from 'react';
import { ThemeProvider } from "@chakra-ui/core";
import { ThemeProvider as StyledProvider } from "styled-components";
import { customTheme } from './utils/theme'
import Home from './components/home/Home';
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import ResetPassword from "./components/resetpassword/ResetPassword";
import Login from "./components/login/Login";
import Dashboard from "./components/dashboard";
import Admin from './components/admin/Admin';
import ProfileDetail from './components/profile/ProfileDetail';

const App = () => {
  return (
    <StyledProvider theme={customTheme}>
    <ThemeProvider theme={customTheme}>
      <Router>
        <Switch>
          <Route path='/' exact component={Home} />
          <Route path="/login/reset" exact component={ResetPassword}/>
          <Route path='/login' component={Login} />
          <Route path='/dashboard' component={Dashboard} />
          <Route path='/admin' component={Admin} />
          <Route path='/profile' component={ProfileDetail} />
        </Switch>
      </Router>
    </ThemeProvider>
    </StyledProvider>
  );
}

export default App;
