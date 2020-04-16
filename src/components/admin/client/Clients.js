import React, { Component } from "react";
import { Link, Route } from "react-router-dom";
import AddClient from "./AddClient";

class Clients extends Component{
    render() {
        return(
            <div className="card text-justify">
                <div className='card-header'>
                    Clients
                </div>
                <div className="card-body">
                    <Link to='/admin/clients/add' className='btn btn-primary'>Add client</Link>
                    <Route path='/admin/clients/add' component={AddClient} />
                </div>
            </div>
        );
    }
}

export default Clients;