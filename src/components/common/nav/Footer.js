import React, { Component } from 'react';

class Footer extends Component{
    render () {
        return (
            <nav className="navbar fixed-bottom navbar-light bg-light">
                <a className="navbar-brand" href="#">&copy; 2015 - {(new Date().getFullYear())} ResourceIdea Limited</a>
            </nav>
        );
    }
}

export default Footer;