import React, { Component } from "react";
import { Link, Route } from "react-router-dom";
import AddEngagement from "./AddEngagement";

class Engagements extends Component{
    render() {
        return(
            <div className="card text-justify">
                <div className='card-header'>
                    Engagements
                </div>
                <div className="card-body">
                    <Link to='/admin/engagements/add' className='btn btn-primary'>Add engagement</Link>
                    <Route path='/admin/engagements/add' component={AddEngagement} />
                </div>
            </div>
        );
    }
}

export default Engagements;