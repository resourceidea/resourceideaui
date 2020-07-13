export function getDashboardData() {
    return (
    Array(22).fill().map(datum => {
        let item = {
            firstName: "John freeboy", lastName: "smith", 
        }
        Array(365).fill().map((d, i) => {
            item[`age${i}`]= '12'
        })
        return item
    })
    )
}

export function getDashboardColumns() {
    return [
        {
            Header: "Showing",
            sticky: 'left',
            columns: [
                {
                    Header: "First Name",
                    accessor: "firstName",
                }
            ]
        },
        {
            Header: "Other Info",
            columns: Array(365).fill().map((day,i) => ({
                Header: "Age",
                accessor: `age${i}`,
                width: '24px'
            }))
        }
    ];
}