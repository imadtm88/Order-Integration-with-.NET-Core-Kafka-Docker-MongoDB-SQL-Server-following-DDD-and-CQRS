import React, { useState, useEffect } from "react";
import axios from "axios";
import {
  Box,
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
  TableContainer,
  Spinner,
  Alert,
  AlertIcon,
  Heading,
  Input,
  Flex,
  Button,
  ButtonGroup,
} from "@chakra-ui/react";

const DataTable = () => {
  const [data, setData] = useState([]);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1); // Track current page

  const itemsPerPage = 4; // Number of items per page

  useEffect(() => {
    axios
      .get("http://localhost:5159/api/upload")
      .then((response) => {
        setData(response.data);
        setLoading(false);
      })
      .catch((error) => {
        setError(error.message);
        setLoading(false);
      });
  }, []);

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" height="100vh">
        <Spinner size="xl" />
      </Box>
    );
  }

  if (error) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" height="100vh">
        <Alert status="error">
          <AlertIcon />
          {error}
        </Alert>
      </Box>
    );
  }

  // Calculate pagination
  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = data.slice(indexOfFirstItem, indexOfLastItem);

  const paginate = (pageNumber) => setCurrentPage(pageNumber);

  const getStatusColor = (status) => {
    if (["PAYMENTFAILURE", "INTEGRATEFAILURE", "CONTROLFAILURE"].includes(status)) {
      return "red";
    } else if (["PAYMENTSUCCESS", "CONTROLESUCCESS", "INTEGRATESUCCESS", "ACCEPTED"].includes(status)) {
      return "#008c6a";
    } else if (["CREATED", "SHIPPED"].includes(status)) {
      return "#1293d6";
    } else {
      return "gray"; // Default color or handle other statuses
    }
  };

  const handleDownloadReport = (orderId) => {
    // Implement your download report logic here
    console.log(`Downloading report for order ID: ${orderId}`);
  };

  return (
    <Box p={4}>
      <Flex direction="column" alignItems="center" mb={4}>
        <Heading as="h1" mb={2}>
          Orders Details
        </Heading>
        <Input placeholder="Search..." width="50%" />
      </Flex>
      <TableContainer>
        <Table variant="striped" colorScheme="teal">
          <Thead>
            <Tr>
              <Th>Reference</Th>
              <Th>Order Status</Th>
              <Th>Last Name</Th>
              <Th>First Name</Th>
              <Th>Product ID</Th>
              <Th>Report</Th> {/* Nouvelle colonne pour le rapport */}
            </Tr>
          </Thead>
          <Tbody>
            {currentItems.map((order, index) => (
              <Tr key={index}>
                <Td>{order.customerOrderRef}</Td>
                <Td>
                  <Box
                    p={1.5}
                    color="white"
                    bg={getStatusColor(order.orderStatus)}
                    borderRadius="md"
                    textAlign="center"
                    width="fit-content"
                    ml={1}
                    borderRightRadius={12}
                    borderLeftRadius={12}
                    borderTopRadius={12}
                  >
                    {order.orderStatus}
                  </Box>
                </Td>
                <Td>{order.lastName}</Td>
                <Td>{order.firstName}</Td>
                <Td>{order.productId}</Td>
                <Td>
                  <Button
                    colorScheme="teal"
                    onClick={() => handleDownloadReport(order.id)} // Appel de la fonction de téléchargement du rapport
                  >
                    Download Report
                  </Button>
                </Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </TableContainer>
      {/* Pagination */}
      <Flex justifyContent="center" mt={4}>
        <ButtonGroup spacing={4}>
          {Array.from({ length: Math.ceil(data.length / itemsPerPage) }).map(
            (item, index) => (
              <Button
                key={index}
                onClick={() => paginate(index + 1)}
                colorScheme={currentPage === index + 1 ? "blue" : "gray"}
                variant={currentPage === index + 1 ? "solid" : "outline"}
              >
                {index + 1}
              </Button>
            )
          )}
        </ButtonGroup>
      </Flex>
    </Box>
  );
};

export default DataTable;