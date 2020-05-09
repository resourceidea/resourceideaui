import React, { Component } from "react";
import { Link, Route } from "react-router-dom";
import AddLinesOfService from "./AddLinesOfService"

class LinesOfService extends Component{
    render() {
        return(
            <div className="card text-justify">
                <div className='card-header'>
                    Lines Of Service
                </div>
                <div className="card-body">
                    <Link to='/admin/lines-of-service/add' className='btn btn-primary'>Add Lines Of Service</Link>
                    <Route path='/admin/lines-of-service/add' component={AddLinesOfService} />
                </div>
            </div>
        );
    }
}

export default LinesOfService;