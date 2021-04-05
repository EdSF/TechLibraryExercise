<template>
  <div class="home">
    <h1>{{ totalItems }} Books</h1>
    <p>Page {{ page }} of {{ totalPages }}</p>

    <!-- Search -->
    <div class="mb-2">

      <p v-if="errors.length" class="alert alert-danger">
        <b>Please correct the following error(s):</b>
        <ul>
          <li v-for="(error,i) in errors" :key="i">{{ error }}</li>
        </ul>
    </p>

      <b-form inline>
        <label class="sr-only" for="search">Search</label>
        <b-form-input
          class="mr-sm-2"
          v-model="searchText"
          type="text"
          id="search"
          placeholder="Search our books..."
        />
        <b-button @click="search" variant="primary">Search</b-button>
      </b-form>
    </div>

    <!-- Pager -->
    <div class="mb-2">
      <b-pagination
        size="md"
        :total-rows="totalItems"
        v-model="page"
        :per-page="perPage"
      ></b-pagination>
    </div>

    <!-- Results table -->
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

    <div class="mb-5">
      <b-button v-if="searchText !== ''" variant="warning" @click="clearSearch" >Show All Books</b-button
      >
    </div>
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
    searchText: "",
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
    errors: [],
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
      let vm = this;
      const baseUrl = "https://localhost:5001/books/";
      let resourceUrl = "";

      if (this.searchText !== "") {
        resourceUrl = `${baseUrl}search/${this.searchText}/`;
      } else {
        resourceUrl = baseUrl;
      }
      // switch (res) {
      //   case "search":
      //     console.info(`the text to search ${this.searchText}`)
      //     resourceUrl = `${baseUrl}search/${this.searchText}/`;
      //     break;
      //   default:
      //     resourceUrl = baseUrl;
      //     break;
      // }
      // console.info(this.page);
      axios.get(`${resourceUrl}${this.page}`).then((response) => {
        vm.page = response.data.page;
        vm.totalPages = response.data.totalPages;
        vm.totalItems = response.data.totalItems;
        vm.previous = response.data.hasPreviousPage;
        vm.next = response.data.hasNextPage;
        vm.items = response.data.books;
      });
    },

    search() {
      this.errors = [];

      if (this.searchText === "") {
        this.errors.push("Nothing to search...");
      }
      if (this.searchText.length < 3) {
        this.errors.push("At least 2 characters needed for search...");
      }

      // console.error(this.errors);

      if (this.errors.length === 0) {
        try {
          this.getData();
        } catch (error) {
          console.error(error);
        }
      }
    },

    clearSearch() {
      this.errors = [];
      this.searchText = "";
      try {
        this.getData();
      } catch (error) {
        console.error(error);
      }
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
      handler: function () {
        // console.info(`Invoked page watch, value ${v}`);
        this.getData();
      },
    },
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>

