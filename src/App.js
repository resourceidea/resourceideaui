import React from 'react';
import './App.css';
import Home from './components/pages/home/Home';
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import Nav from './components/common/nav/Nav';
import Dashboard from "./components/pages/dashboard/Dashboard";
import Admin from './components/pages/admin/Admin';
import ProfileDetail from './components/pages/profile/ProfileDetail';

const App = () => {
  return (
    <Router>
      <div className="App">
        <Nav />
        <Switch>
          <Route path='/' exact component={Home} />
          <Route path='/dashboard' component={Dashboard} />
          <Route path='/admin' component={Admin} />
          <Route path='/profile' component={ProfileDetail} />
        </Switch>
      </div>
    </Router>
  );
}

export default App;
