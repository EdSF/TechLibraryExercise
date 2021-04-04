<template>
  <div class="home">
    <h1>{{ msg }}</h1>
    <ul>
      <li>Result: {{ totalItems }}</li>
      <li>Page {{ page }} of {{ totalPages }}</li>
    </ul>

    <div>
      <b-pagination
        size="md"
        :total-rows="totalItems"
        v-model="page"
        :per-page="perPage"
      ></b-pagination>
    </div>

    <b-table
      striped
      hover
      :items="items"
      :fields="fields"
      :current-page="page"
      :per-page="0"
      responsive="sm"
    >
      <template v-slot:cell(thumbnailUrl)="data">
        <b-img :src="data.value" thumbnail fluid></b-img>
      </template>
      <template v-slot:cell(title_link)="data">
        <b-link :to="{ name: 'book_view', params: { id: data.item.bookId } }">{{
          data.item.title
        }}</b-link>
      </template>
    </b-table>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "Home",
  props: {
    msg: String,
  },
  data: () => ({
    page: 1,
    totalPages: Number,
    totalItems: 0,
    perPage: 10,

    previous: Boolean,
    next: Boolean,
    fields: [
      {
        key: "thumbnailUrl",
        label: "Book Image",
      },
      {
        key: "title_link",
        label: "Book Title",
        sortable: true,
        sortDirection: "desc",
      },
      {
        key: "isbn",
        label: "ISBN",
        sortable: true,
        sortDirection: "desc",
      },
      {
        key: "descr",
        label: "Description",
        sortable: true,
        sortDirection: "desc",
      },
    ],
    items: [],
  }),

  methods: {
    // dataContext(ctx, callback) {
    //   var vm = this;
    //   console.info(this.page);
    //   axios.get("https://localhost:5001/books").then((response) => {
    //     console.info(response);
    //     vm.page = response.data.page;
    //     vm.totalPages = response.data.totalPages;
    //     vm.previous = response.data.hasPreviousPage;
    //     vm.next = response.data.hasNextPage;
    //     vm.totalItems = response.data.totalItems;
    //     callback(response.data.books);
    //   });
    // },

    getData() {
      var vm = this;
      // console.info(this.page);
      axios
        .get(`https://localhost:5001/books/${this.page}`)
        .then((response) => {
          vm.page = response.data.page;
          vm.totalPages = response.data.totalPages;
          vm.totalItems = response.data.totalItems;
          vm.previous = response.data.hasPreviousPage;
          vm.next = response.data.hasNextPage;
          vm.items = response.data.books;
        });
    },
  },

  mounted() {
    try {
      this.getData();
    } catch (error) {
      console.error(error);
    }
  },

  watch: {
    page: {
      handler: function(){
        // console.info(`Invoked page watch, value ${v}`);
        this.getData();
      }
    }
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>

