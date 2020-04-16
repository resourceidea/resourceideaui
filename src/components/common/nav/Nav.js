import React, { Component } from 'react';
import { Link } from 'react-router-dom';

class Nav extends Component{
    render () {
        return (
            <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <Link className="navbar-brand" to="/">ResourceIdea</Link>
            <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarText" aria-controls="navbarText" aria-expanded="false" aria-label="Toggle navigation">
              <span className="navbar-toggler-icon"></span>
            </button>
            <div className="collapse navbar-collapse" id="navbarText">
              <ul className="navbar-nav mr-auto">
<li className="nav-item">
                  <Link className="nav-link" to='/dashboard'>Dashboard</Link>
                </li>
                <li className="nav-item">
                  <Link className="nav-link" to='/admin'>Admin</Link>
                </li>
              </ul>
              <span className="navbar-text">
                <ul className="navbar-nav">
 <li className="nav-item">
                  <Link className="nav-link" to='/login'>Login</Link>
                </li>
                    <li className="nav-item">
                        <Link className="nav-link" to="/profile">My Profile</Link>
                    </li>
                </ul>
              </span>
            </div>
          </nav>
        );
    }
}

export default Nav;