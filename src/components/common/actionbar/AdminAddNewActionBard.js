import React, { Component } from "react";
import { Link } from "react-router-dom";
import { capitalizeFirstLetter } from "../../../utils/Common";

class AdminAddNewActionBar extends Component{
    render(){
        return(
            <div>
                <h5>New {capitalizeFirstLetter(this.props.page)}</h5>
                <nav className='navbar navbar-expand-sm navbar-light bg-light border border-secondary'>
                    <ul className='navbar-nav mr-auto'>
                        <li className='nav-item'>
                            <Link to={this.props.dashboardLink} className='btn btn-light btn-sm text-left'>Dashboard</Link>&nbsp;
                        </li>
                        <li className='nav-item'>
                            <Link to={this.props.listLink} className='btn btn-light btn-sm text-left'>{capitalizeFirstLetter(this.props.page)} List</Link>
                        </li>
                    </ul>
                </nav>
                <hr />
            </div>
        );
    }
}

export default AdminAddNewActionBar;