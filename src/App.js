import React from 'react';
import './App.css';
import Home from './components/home/Home';
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import Nav from './components/common/nav/Nav';
import ResetPassword from "./components/resetpassword/ResetPassword";
import Login from "./components/login/Login";
import Dashboard from "./components/dashboard/Dashboard";
import Admin from './components/admin/Admin';
import ProfileDetail from './components/profile/ProfileDetail';
import Footer from "./components/common/nav/Footer";

const App = () => {
  return (
    <Router>
      <div className="App">
        <Nav />
        <Switch>
          <Route path='/' exact component={Home} />
          <Route path="/login/reset" exact component={ResetPassword}/>
          <Route path='/login' component={Login} />
          <Route path='/dashboard' component={Dashboard} />
          <Route path='/admin' component={Admin} />
          <Route path='/profile' component={ProfileDetail} />
        </Switch>
        <Footer />
      </div>
    </Router>
  );
}

export default App;
