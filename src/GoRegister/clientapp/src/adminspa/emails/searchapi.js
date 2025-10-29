import axios from "axios";

const getUsersIdsFromEmails = async (emails) => {
  return new Promise((resolve, reject) => {
    axios.post('/api/emails/emailLookup', { values: emails })
    .then(function (response) {
      resolve(response.data);
    })
    .catch(function (error) {
      reject(error);
    }) 
  });
};

const searchUsers = async (input) => {
  return new Promise((resolve, reject) => {
    if (input.length < 3) {
      return resolve([]);
    }

    axios.get('/api/emails/searchUsers', {
      params: {
        value: input
      }
    })
    .then(function (response) {
      resolve(response.data);
    })
    .catch(function (error) {
      reject(error);
    })
  });
};

const customSearch = (filters) => {
  return new Promise((resolve, reject) => {
    axios.post('/api/emails/customSearchUsers', {
        filters
    })
    .then(function (response) {
      resolve(response.data);
    })
    .catch(function (error) {
      reject(error);
    })
  });
};

export { getUsersIdsFromEmails, searchUsers, customSearch };
