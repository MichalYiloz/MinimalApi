import axios from 'axios';
axios.defaults.baseURL = "http://localhost:5202";

const apiUrl = "http://localhost:5202"

export default {
  //get
  getTasks: async () => {
    const result = await axios.get(`/`)
    return result.data;
  },
//post
  addTask: async (Name) => {
    debugger
    console.log('addTask', Name)
    const result = await axios.post(`/ToAdd/${Name}`)
  },
//put
  setCompleted: async (id, IsComplete) => {
    const result = await axios.put(`/ToPut/${id}/${IsComplete}`)
    return {};
  },
 //delete 
  deleteTask: async (id) => {
    const result = await axios.delete(`/ToDelete/${id}`)
    return result.data;
  },

};
axios.interceptors.response.use(
  response => response,
  error => {
    console.log('Error:', error);

    window.location.href = '/';
  });